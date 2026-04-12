using EventScheduler.Interfaces;
using EventScheduler.Models;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventScheduler.Repositories
{
    
    // File-based implementation of IEventRepository. Responsible for persisting events into a JSON file
    
    public class FileEventRepository : IEventRepository
    {
        // Stores the full file path of the JSON file
        private readonly string _filePath;

        
        ///Constructor initializes file storage using configuration. Ensures required folder and file exist.
        public FileEventRepository(IOptions<StorageOptions> options)
        {
            // Read configuration values from appsettings.json
            var config = options.Value;

            // Get the base path 
            var basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

            // Combine base path with configured folder name 
            var folderPath = Path.Combine(basePath, config.Folder);

            // Ensure the directory exists (create if it doesn't)
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Build full file path
            _filePath = Path.Combine(folderPath, config.FileName);

            // If file doesn't exist, create it with an empty JSON array
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        
        // Retrieves all events from the JSON file.
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            // Read JSON content from file
            var json = await File.ReadAllTextAsync(_filePath);

            // Deserialize JSON into list of Event objects. Return empty list if deserialization fails or file is empty
            return JsonSerializer.Deserialize<List<Event>>(json)
                   ?? new List<Event>();
        }

        // Saves all events to the JSON file.
        public async Task SaveAllAsync(IEnumerable<Event> events)
        {
            // Convert events list into formatted JSON string
            var json = JsonSerializer.Serialize(events, new JsonSerializerOptions
            {
                WriteIndented = true 
            });

            // Write JSON content to file (overwrites existing data)
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}