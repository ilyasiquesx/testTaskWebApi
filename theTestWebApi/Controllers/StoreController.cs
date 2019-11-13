using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using theTestWebApi.Models;

namespace theTestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        StoreContext _db;
        public StoreController(StoreContext db)
        {
            _db = db;
        }
         
        // GET: api/Store Возвращает все товары из таблицы "Products".
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get() 
        {

            return Ok(_db.Products.ToList());
        }


        // GET: api/Store/id Возвращает конкретный товар из таблицы "Products".
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            Product prod = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if(prod!=null)
            {
                return Ok(prod);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Store Создает запись в таблице "Orders", т.е. создается заказ.
        [HttpPost]
        public ActionResult<Order> Post([FromBody] Order order)
        {
            Product prod = _db.Products.FirstOrDefault(p => p.ProductId == order.ProductId);
            if (prod != null)
            {
                order.Date = DateTime.Now;
                _db.Orders.Add(order);
                _db.SaveChanges();
                return CreatedAtAction("Get", new Order { ProductId = order.ProductId, BuyersName = order.BuyersName, Date = order.Date, OrderId = order.OrderId });
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Store/addproduct Создает запись в таблице "Products".

        [HttpPost("addproduct")]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return CreatedAtAction("Get", new Product { ProductId = product.ProductId, Cost = product.Cost, Name = product.Name });
        }

        // DELETE: api/store/deleteproduct/id Удаляет запись из таблицы "Product", а также удаляет связанные записи из таблицы "Orders".
        [HttpDelete("deleteproduct/{id}")]
        public ActionResult<Product> DeleteProduct(int id)
        {
            Product product = _db.Products.FirstOrDefault(p=>p.ProductId==id);
            if(product != null)
            {
                _db.Products.Remove(product);
                if (_db.Orders.Count() != 0)
                {
                    _db.Orders.RemoveRange(_db.Orders.Where(o => o.ProductId == product.ProductId));
                }
                _db.SaveChanges();
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
        
        // DELETE: api/store/deleteorder/id Удаляет запись из таблицы "Orders".
        [HttpDelete("deleteorder/{id}")]
        public ActionResult<Order> DeleteOrder(int id)
        {
            Order order = _db.Orders.FirstOrDefault(o => o.OrderId == id);
            if(order!=null)
            {
                _db.Orders.Remove(order);
                _db.SaveChanges();
                return Ok(order);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
