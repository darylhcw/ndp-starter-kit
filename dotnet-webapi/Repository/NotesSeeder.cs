
using Microsoft.EntityFrameworkCore;
using DotnetWebAPI.Models;

namespace DotnetWebAPI.Repository {

    public static class NotesSeeder {

        public static void AddNotes(IApplicationBuilder app) {

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try {
                var context = services.GetRequiredService<NotesDbContext>();
                context.Database.Migrate();

                var testNote = context.Notes.FirstOrDefault(n => n.Id == 1);
                if (testNote == null) {
                    context.Notes.Add(new Note("Default test note!"));
                }
                context.SaveChanges();

            } catch (Exception ex) {
                Console.WriteLine($"Exception when seeding data {nameof(NotesSeeder)}:" + ex.Message);
            }
        }
    }
}