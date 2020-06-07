using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APINJ.Controllers
{
    public class UserController : ApiController
    {

        [HttpGet]
        [Route("getUsers")]
        public IHttpActionResult getUsers()
        {
            using(Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                IList<Models.ModelRequest.Users> listUsers = null;

                listUsers = db.USERS.Select(u => new Models.ModelRequest.Users()
                {
                    U_Id = u.U_Id,
                    U_Name = u.U_Name,
                    U_Apellidos = u.U_Apellidos,
                    U_Nick = u.U_Nick,
                    U_Email = u.U_Email,
                    U_Pass = u.U_Pass,
                    U_Status = u.U_Status,
                    U_Gender = u.U_Gender
                }).ToList<Models.ModelRequest.Users>();

                if (listUsers != null && listUsers.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listUsers);
            }
        }

        [HttpGet]
        [Route("getUserById")]
        public IHttpActionResult GetUsersById(int id)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                try
                {

                    Models.USERS _user = new Models.USERS();

                    _user = db.USERS.FirstOrDefault(u => u.U_Id == id);

                    if (_user == null)
                        return NotFound();

                    return Ok(_user);
                }
                catch (Exception)
                {

                    return BadRequest("Ha ocurrido un error al buscar el usuario");
                }
            }
        }

        [HttpGet]
        [Route("getUsersByCredentials")]
        public IHttpActionResult GetUsersByCredentials(String pass, String nickOrEmail)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                Models.USERS _user = new Models.USERS();

                try
                {
                    _user = db.USERS.FirstOrDefault(u => u.U_Pass == pass && (u.U_Email == nickOrEmail || u.U_Nick == nickOrEmail));

                    if (_user == null)
                        return NotFound();

                    if (_user.U_Status != "A")
                    {
                        return BadRequest("Usuario Inactivo, Comuníquese con el administrador");
                    }

                    return Ok(_user);
                }
                catch (Exception)
                {

                    return BadRequest("Ha ocurrido un error en la autenticación del usuario");
                }
            }
        }

        [HttpPost]
        [Route("addUser")]
        public IHttpActionResult AddUser(Models.ModelRequest.Users user)
        {

            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {

                try
                {
                    if (user.U_Name != null)
                    {
                        Models.USERS _User = new Models.USERS
                        {
                            U_Id = 0,
                            U_Name = user.U_Name,
                            U_Apellidos = user.U_Apellidos,
                            U_Nick = user.U_Nick,
                            U_Email = user.U_Email,
                            U_Pass = user.U_Pass,
                            U_Gender = user.U_Gender
                        };
                        
                        _User.U_Status = "A";

                        db.USERS.Add(_User);
                        db.SaveChanges();
                    } else
                    {
                        return BadRequest("Debe completar los datos para finalizar el registro");
                    }
                }
                catch (Exception)
                {

                    return BadRequest("No se puedo insertar el registro");
                }
            }

            return Ok("OK");
        }

        [HttpPost]
        [Route("changeStatusUser")]
        public IHttpActionResult ChangeTheStatusOfTheUser(int id)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                try
                {
                    Models.USERS _user = new Models.USERS();

                    _user = db.USERS.FirstOrDefault(u => u.U_Id == id);

                    if (_user == null)
                        return NotFound();

                    _user.U_Status = (_user.U_Status == "A") ? "I" : "A";

                    db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    return BadRequest("Ha ocurrido un error al cambiar de estado");
                }
               
            }

            return Ok("OK");
        }

        [HttpPost]
        [Route("updateUser")]
        public IHttpActionResult UpdateUser(Models.ModelRequest.Users user)
        {
            using (Models.NJFOODEntities db = new Models.NJFOODEntities())
            {
                try
                {
                    Models.USERS _user = new Models.USERS();

                    _user = db.USERS.FirstOrDefault(u => u.U_Nick == user.U_Nick && u.U_Email != user.U_Email);

                    if (_user != null)
                    {
                        return BadRequest("Ya existe un usuario con este Alias");
                    }

                    _user = db.USERS.FirstOrDefault(u => u.U_Id == user.U_Id);

                    _user.U_Name = user.U_Name;
                    _user.U_Apellidos = user.U_Apellidos;
                    _user.U_Nick = user.U_Nick;
                    _user.U_Email = user.U_Email;
                    _user.U_Pass = user.U_Pass;
                    _user.U_Gender = user.U_Gender;

                    db.Entry(_user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    return BadRequest("Ha ocurrido un error al actualizar el usuario");
                }
            }
            return Ok("Usuario actualizado Satisfactoriamente");
        }
    }
}
