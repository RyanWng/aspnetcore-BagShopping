using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace QualityBags.Controllers
{
    [Authorize(Roles =("Customer,Admin"))]
    public class CustomerOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CustomerOrders
        /// <summary>
        /// Return all orders of current user
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var userOrders = (from orders in _context.Orders where orders.Customer == user orderby orders.Date select orders).ToList();

            return View(userOrders);
        }

        // GET: CustomerOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create an order with user's input
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,City,Contact,Country,PostalCode,Receiver,Street")] Order order)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                //Get items in the cart
                ShoppingCart userCart = ShoppingCart.GetCart(this.HttpContext);
                ICollection<CartItem> cartItems = userCart.GetCartItems(_context);

                //Create order
                List<OrderItem> orderItems = new List<OrderItem>();
                foreach(CartItem item in cartItems)
                {
                    OrderItem orderItem = CreateOneOrderItem(item);
                    orderItem.Order = order;
                    orderItems.Add(orderItem);
                    _context.OrderItem.Add(orderItem);
                }

                order.Customer = currentUser;
                order.TotalCost = ShoppingCart.GetCart(this.HttpContext).GetCartTotal(_context);
                order.SubTotal = order.TotalCost * (1 - order.GST);
                order.Date = DateTime.Today;
                order.Status = OrderStatus.Waiting;
                order.OrderItems = orderItems;
                _context.Add(order);

                await _context.SaveChangesAsync();

                return RedirectToAction("OrderDetails", new RouteValueDictionary(
                    new { id = order.OrderID }));
            }
            return View(order);
        }

        /// <summary>
        /// Return details of an order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            Order order = await _context.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.OrderID == id);
            if (order != null)
            {
                ShoppingCart.GetCart(this.HttpContext).EmtypCart(_context);
                return View(order);
            }
            else
            {
                return NotFound();
            }
        }


        // GET: CustomerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }





        // GET: CustomerOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: CustomerOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,City,Contact,Country,CustomerID,Date,PostalCode,Receiver,Status,Street,SubTotal,TotalCost")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: CustomerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: CustomerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        #region Helpers
        private OrderItem CreateOneOrderItem(CartItem cartItem)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.Bag = cartItem.Bag;
            orderItem.UnitPrice = cartItem.Bag.Price;
            orderItem.Quantity = cartItem.Count;
            return orderItem;
        }
        #endregion
    }
}
