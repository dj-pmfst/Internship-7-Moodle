using Moodle.Domain.Entities;
using Moodle.Domain.Enums;

namespace Moodle.Infrastructure.Persistence
{
    public static class DataSeed
    {
        public static async Task SeedAsync(MoodleDbContext context)
        {
            if (context.Users.Any())
                return;

            var admin = new User
            {
                Name = "Admin Hasenbegović",
                Email = "admin@moodle.com",
                Password = "Admin123!",
                Role = Roles.admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var prof1 = new User
            {
                Name = "Ivan Ivanović",
                Email = "i.ivan@moodle.com",
                Password = "Prof123!",
                Role = Roles.profesor,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var prof2 = new User
            {
                Name = "Maria Marić",
                Email = "m.maric@moodle.com",
                Password = "Prof123!",
                Role = Roles.profesor,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var student1 = new User
            {
                Name = "Mate Matić",
                Email = "mmat@student.com",
                Password = "Student123!",
                Role = Roles.student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var student2 = new User
            {
                Name = "Ivo Ivić",
                Email = "ivo@student.com",
                Password = "Student123!",
                Role = Roles.student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var student3 = new User
            {
                Name = "Ana Anić",
                Email = "ana@student.com",
                Password = "Student123!",
                Role = Roles.student,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(admin, prof1, prof2, student1, student2, student3);
            await context.SaveChangesAsync();

            var course1 = new Course
            {
                Name = "Uvod u programiranje",
                Description = "uvod u programiranje",
                ProfessorId = prof1.Id,
                CreatedAt = DateTime.UtcNow
            };

            var course2 = new Course
            {
                Name = "Matematika 1",
                Description = "algebra",
                ProfessorId = prof1.Id,
                CreatedAt = DateTime.UtcNow
            };

            var course3 = new Course
            {
                Name = "Fizika 1",
                Description = "mehanika i kinemtaika",
                ProfessorId = prof2.Id,
                CreatedAt = DateTime.UtcNow
            };

            context.Courses.AddRange(course1, course2, course3);
            await context.SaveChangesAsync();


            var enrollment1 = new CourseEnrollment
            {
                StudentId = student1.Id,
                CourseId = course1.Id
            };

            var enrollment2 = new CourseEnrollment
            {
                StudentId = student1.Id,
                CourseId = course2.Id
            };

            var enrollment3 = new CourseEnrollment
            {
                StudentId = student2.Id,
                CourseId = course1.Id
            };

            var enrollment4 = new CourseEnrollment
            {
                StudentId = student2.Id,
                CourseId = course3.Id
            };

            var enrollment5 = new CourseEnrollment
            {
                StudentId = student3.Id,
                CourseId = course2.Id
            };

            context.Enrollments.AddRange(enrollment1, enrollment2, enrollment3, enrollment4, enrollment5);
            await context.SaveChangesAsync();


            var announcement1 = new Announcement
            {
                CourseId = course1.Id,
                ProfessorId = prof1.Id,
                Title = "Dobrodošli u kolegij",
                Text = "Uvodni sat 15.5.",
                CreatedAt = DateTime.UtcNow.AddDays(-28)
            };

            var announcement2 = new Announcement
            {
                CourseId = course1.Id,
                ProfessorId = prof1.Id,
                Title = "Prvi domaći",
                Text = "rok predaje tjedan dana",
                CreatedAt = DateTime.UtcNow.AddDays(-14)
            };

            var announcement3 = new Announcement
            {
                CourseId = course2.Id,
                ProfessorId = prof1.Id,
                Title = "Raspored ispita",
                Text = "U prilogu je raspored ispita",
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            };

            context.Announcements.AddRange(announcement1, announcement2, announcement3);
            await context.SaveChangesAsync();


            var material1 = new Material
            {
                CourseID = course1.Id,
                Name = "Uvod",
                Url = "https://example.com/lecture1.pdf",
                CreatedAt = DateTime.UtcNow.AddDays(-27)
            };

            var material2 = new Material
            {
                CourseID = course1.Id,
                Name = "algebra",
                Url = "https://example.com/lecture2.pdf",
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            };

            var material3 = new Material
            {
                CourseID = course2.Id,
                Name = "Mehanika",
                Url = "https://example.com/lecture3.pdf",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            };

            context.Materials.AddRange(material1, material2, material3);
            await context.SaveChangesAsync();


            var message1 = new Message
            {
                SenderId = student1.Id,
                ReceiverId = prof1.Id,
                Text = "Bok.",
                SentAt = DateTime.UtcNow.AddDays(-5)
            };

            var message2 = new Message
            {
                SenderId = prof1.Id,
                ReceiverId = student1.Id,
                Text = "Pozdrav",
                SentAt = DateTime.UtcNow.AddDays(-5).AddHours(1)
            };

            var message3 = new Message
            {
                SenderId = student2.Id,
                ReceiverId = student3.Id,
                Text = "Kad je domaći",
                SentAt = DateTime.UtcNow.AddDays(-3)
            };

            context.Messages.AddRange(message1, message2, message3);
            await context.SaveChangesAsync();
        }
    }
}