// Generate BCrypt hash for password123
string password = "password123";
string hash = BCrypt.Net.BCrypt.HashPassword(password, 12);
Console.WriteLine($"Password: {password}");
Console.WriteLine($"BCrypt Hash: {hash}");
Console.WriteLine();
Console.WriteLine("SQL UPDATE statement:");
Console.WriteLine($"UPDATE PERSONNEL SET password_hash = '{hash}' WHERE password_hash IS NULL OR password_hash = '';");
