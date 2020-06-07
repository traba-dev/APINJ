using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APINJ.Controllers
{
    public class FoodController : ApiController
    {
        [HttpGet]
        [Route("getFoodsAd")]
        public IHttpActionResult getFoodsAd()
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                IList<Models.ModelRequest.Food> foods = null;

                foods = db.FOOD.Select(u => new Models.ModelRequest.Food()
                {
                    id = u.id,
                    name = u.name,
                    description = u.description,
                    price = u.price,
                    img = u.img,
                    category = u.CATEGORY_FOOD.id,
                    status = u.status

                }).ToList<Models.ModelRequest.Food>();

                if (foods == null)
                {
                    return NotFound();
                }
                return Ok(foods);
            }
        }

        [HttpGet]
        [Route("getFoodsAdByCategory")]
        public IHttpActionResult getFoodsByCategoryAd(int id)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                IList<Models.ModelRequest.Food> foods = null;

                foods = (from p in db.FOOD.Select(u => new Models.ModelRequest.Food()
                {
                    id = u.id,
                    name = u.name,
                    description = u.description,
                    price = u.price,
                    img = u.img,
                    category = u.CATEGORY_FOOD.id,
                    status = u.status

                })
                         where (p.category == id)
                         select p).ToList<Models.ModelRequest.Food>();

                if (foods == null)
                {
                    return NotFound();
                }


                return Ok(foods);
            }
        }

        [HttpGet]
        [Route("getFoodById")]
        public IHttpActionResult getFoodById(int id)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities()) 
            {
                try
                {
                    Models.FOOD food = null;
                    food = db.FOOD.FirstOrDefault(u => u.id == id);

                    if (food == null)
                    {
                        return NotFound();
                    }

                    Models.ModelRequest.Food foodReturn = new Models.ModelRequest.Food
                    {
                        id = food.id,
                        name = food.name,
                        description = food.description,
                        price = food.price,
                        img = food.img,
                        status = food.status,
                        category = food.CATEGORY_FOOD.id
                    };

                    return Ok(foodReturn);
                }
                catch (Exception)
                {

                    return BadRequest("Ocurrió un error al recuperar la comida");
                }
                
            }
        }

        [HttpPost]
        [Route("updateFood")]
        public IHttpActionResult updateFood(Models.ModelRequest.Food food)
        {
            try
            {
                using (Models.NJFOODEntities db = new Models.NJFOODEntities())
                {

                    Models.FOOD dbFood = new Models.FOOD();

                    dbFood = db.FOOD.FirstOrDefault(u => u.id == food.id);

                    if (dbFood == null)
                    {
                        return NotFound();
                    }

                    dbFood.name = food.name;
                    dbFood.price = food.price;
                    dbFood.description = food.description;
                    dbFood.status = food.status;

                    db.Entry(dbFood).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }


                return Ok("Registro actualizado satisfactoriamente");
            }
            catch (Exception)
            {

                return BadRequest("Ocurrió un error al conectarse al servidor");
            }

            
        }

        [HttpGet]
        [Route("addFood")]
        public IHttpActionResult addFood(Models.ModelRequest.Food food)
        {
            try
            {
                using (Models.NJFOODEntities db = new Models.NJFOODEntities())
                {
                    
                    Models.CATEGORY_FOOD category = new Models.CATEGORY_FOOD();

                    category = db.CATEGORY_FOOD.FirstOrDefault(u => u.id == food.category);

                    if (category == null)
                    {
                        return NotFound();
                    }
                    Models.FOOD dbFood = new Models.FOOD
                    {
                        id = 0,
                        name = food.name,
                        description = food.description,
                        price = food.price,
                        img = "",
                        status = "A",
                        CATEGORY_FOOD = category
                    };


                    db.FOOD.Add(dbFood);
                    db.SaveChanges();

                    return Ok("Comida agregada Satisfactoriamente");
                }
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al agregar la comida");
                throw;
            }
        }
    }
}
