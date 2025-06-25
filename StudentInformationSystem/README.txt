# Student Information System

A basic enhanced Student Information System (SIS) built with **VB.NET Web Forms** and **Supabase (PostgreSQL)**. This system enables role-based management of students, courses, enrollments, and dashboards for tracking course statistics.

---

## 🎯 Features

- ✅ **Role-Based Login (Admin / Student)**
- 👥 **Student Management** – Add, edit, and delete student records
- 📘 **Course Management** – Create, update, and delete courses
- 📝 **Enrollments** – Enroll students in courses, with duplicate check
- 📊 **Dashboard** – Visualize student enrollments using Chart.js
- 👤 **My Profile Page** – View student info and enrolled courses

---

## 🧰 Technology Stack

| Layer        | Technology              |
|--------------|--------------------------|
| Frontend     | ASP.NET Web Forms (.NET 4.7.2), Bootstrap |
| Backend      | VB.NET                   |
| Database     | PostgreSQL (via Supabase) |
| Charting     | Chart.js                 |
| Security     | Session-based auth, bcrypt hash (via `crypt()` in PostgreSQL) |

---

## 🚀 How to Run Locally

1. Open the project in **Visual Studio**  
2. Restore NuGet packages if needed  
3. Set `Login.aspx` or `Default.aspx` as the start page  
4. Ensure Supabase connection string in `Web.config` is valid  
5. Run the application via IIS Express  

---

## 📁 Folder Structure

- `/ManageStudents.aspx` – Full CRUD for student records (Admin only)
- `/ManageCourses.aspx` – Course CRUD with validation (Admin only)
- `/ManageEnrollments.aspx` – Enroll students in courses
- `/Dashboard.aspx` – Chart of course enrollments
- `/MyProfile.aspx` – Student details + enrolled courses
- `/Site.master` – Master layout + dynamic navbar

---

## ✅ Admin Access

Admins must be registered manually in the database with a linked student record.

---

## 📌 Known Issues & Limitations

See `SystemDesign.pdf` or `KnownIssues.md` for a complete list of known limitations (e.g. no password reset, no admin registration UI, no logging/auditing).

---

## 📄 Documentation

- `UserManual.pdf`  
- `SystemDesign.pdf`  
- `Presentation.pptx`  
- `README.md`

---

## 👩‍💻 Author

Nora Borgen Hausberg  
Prepared for the **Programming with Generative AI** capstone  
Supervised by Mario Silic, PhD
