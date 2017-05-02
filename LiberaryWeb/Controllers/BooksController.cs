using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryWeb;

namespace LiberaryWeb.Controllers
{
    public class BooksController : Controller
    {
        private Library db = new Library();

        // GET: Books
        public ActionResult Index()
        {
            //var books = db.Books.Include(b => b.Author);
            return View();
        }

        public ActionResult GetAllBooks()
        {
            List<BookViewModel> bookVM = new List<BookViewModel>();
            foreach (var item in db.Books.ToList())
            {
                bookVM.Add(new BookViewModel() { ID = item.ID, AuthorID = item.AuthorID,AuthorName=item.Author.Name, BookName = item.BookName, Description = item.Description });
            }
            return Json(bookVM, JsonRequestBehavior.AllowGet);
        }






        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }



        // GET: Books/Details/ for Modal pop up window 
        public ActionResult GetBookDetailsForModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            BookViewModel bookVM = new BookViewModel() { ID = book.ID, AuthorID = book.AuthorID, AuthorName = book.AuthorName, BookName = book.BookName, Description = book.Description };


            return Json(bookVM, JsonRequestBehavior.AllowGet);
        }







        // GET: Books/Create
        public ActionResult Create()
        {

           //// ViewBag.AuthorID = new SelectList(db.Authors, "Id", "Name");
                     
            var authorList = db.Authors.ToList();  //give UI author List
            authorList.Insert(0, new Author { Id = 0, Name = "..." }); //then insert new row 
            ViewBag.AuthorID = new SelectList(authorList, "Id", "Name", string.Empty);// string.Empty will bring new line in DDL when creating next time. 

            
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        
        public ActionResult Create(Book book)
        {
            ResponseMessage responseMessage;
            if (string.IsNullOrEmpty(book.AuthorID.ToString()))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "AuthorID is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(book.BookName))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Book Name is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            var checkAuthorID = db.Books.Where(e => e.AuthorID == book.AuthorID).ToList();
            var checkBookName = db.Books.Where(e => e.BookName == book.BookName).ToList();
            if (checkAuthorID.Count != 0)
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Name " + book.AuthorID + " already exists."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (checkBookName.Count != 0)
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Email " + book.BookName + " already exists."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }

            try
            {
                ////////=== We are adding new record into database
                db.Books.Add(book);
                db.SaveChanges();

                responseMessage = new ResponseMessage()
                {
                    Success = true,
                    Message = "Books information successfully saved."
                };

            }
            catch (Exception)
            {

                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Something went wrong."
                };
            }
            return Json(responseMessage, JsonRequestBehavior.AllowGet);
            
    }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "Id", "Name", book.AuthorID);
            
            return View(book);

        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            ResponseMessage responseMessage;
            if (book.ID <= 0)
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "book id is empty."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(book.AuthorID.ToString()))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Author is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(book.BookName))
            {
                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "BookName is required."
                };
                return Json(responseMessage, JsonRequestBehavior.AllowGet);
            }
            try
            {
                
                ////=== we are updating existing record
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();

                responseMessage = new ResponseMessage()
                {
                    Success = true,
                    Message = "Books information successfully saved."
                };

            }
            catch (Exception)
            {

                responseMessage = new ResponseMessage()
                {
                    Success = false,
                    Message = "Something went wrong."
                };
            }
            return Json(responseMessage, JsonRequestBehavior.AllowGet);

        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);

        }

       

    }
}
