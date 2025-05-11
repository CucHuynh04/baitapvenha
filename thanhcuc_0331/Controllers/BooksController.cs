using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhcuc_0331.Models;

namespace thanhcuc_0331.Controllers
{
    public class BooksController : Controller
    {
        // Phương thức lấy danh sách sách từ Session hoặc khởi tạo mặc định
        private List<Book> GetBooks()
        {
            if (Session["Books"] == null)
            {
                Session["Books"] = new List<Book>
            {
                new Book { Id = 1, Title = "Sách 1", Author = "Tác giả 1", PublicYear = 2020, Price = 400, Cover = "Content/images/book1.jpg" },
                new Book { Id = 2, Title = "Sách 2", Author = "Tác giả 2", PublicYear = 2021, Price = 500, Cover = "Content/images/book2.jpg" },
                new Book { Id = 3, Title = "Sách 3", Author = "Tác giả 3", PublicYear = 2022, Price = 600, Cover = "Content/images/book3.jpg" }
            };
            }
            return (List<Book>)Session["Books"];
        }

        public ActionResult ListBooks(string searchString, string sortOrder)
        {
            ViewBag.TitlePageName = "Book view page";
            ViewBag.CurrentFilter = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            var books = GetBooks();

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }

            // Sắp xếp
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Title).ToList();
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price).ToList();
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price).ToList();
                    break;
                default:
                    books = books.OrderBy(b => b.Title).ToList();
                    break;
            }

            return View(books);
        }
        public ActionResult Detail(int? id)
        {
            var books = GetBooks();  // Lấy danh sách sách từ session hoặc dữ liệu mặc định
            Book book = books.FirstOrDefault(s => s.Id == id);  // Tìm sách theo id

            if (book == null)
            {
                return HttpNotFound();  // Nếu không tìm thấy sách, trả về lỗi 404
            }

            return View(book);  // Trả về view với sách được tìm thấy
        }

        public ActionResult Edit(int? id)
        {
            // Kiểm tra xem id có null không
            if (id == null)
            {
                return HttpNotFound(); // Nếu không có id, trả về lỗi 404
            }

            var books = GetBooks(); // Lấy danh sách sách từ session
            var book = books.FirstOrDefault(b => b.Id == id); // Tìm sách theo id

            if (book == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy sách
            }

            return View(book); // Trả về view với sách cần chỉnh sửa
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                var books = GetBooks(); // Lấy danh sách sách từ session
                var editBook = books.FirstOrDefault(b => b.Id == book.Id); // Tìm sách theo Id

                if (editBook == null)
                {
                    return HttpNotFound(); // Nếu không tìm thấy sách
                }

                // Cập nhật thông tin sách
                editBook.Title = book.Title;
                editBook.Author = book.Author;
                editBook.PublicYear = book.PublicYear;
                editBook.Price = book.Price;
                editBook.Cover = book.Cover;

                Session["Books"] = books; // Cập nhật lại session với danh sách sách mới

                return RedirectToAction("ListBooks"); // Chuyển hướng về trang danh sách sách
            }

            // Nếu có lỗi, trả về lại view
            return View(book);
        }



        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                var books = GetBooks();
                book.Id = books.Max(b => b.Id) + 1;
                books.Add(book);
                Session["Books"] = books; // Cập nhật lại Session

                return RedirectToAction("ListBooks");
            }

            return View(book);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            var books = GetBooks();
            Book book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return HttpNotFound();

            return View(book);  // Trả về view để xác nhận xóa
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var books = GetBooks();
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book); // Xóa sách khỏi danh sách
                Session["Books"] = books; // Cập nhật lại session
            }
            return RedirectToAction("ListBooks"); // Quay lại danh sách sách
        }
    }
    }
