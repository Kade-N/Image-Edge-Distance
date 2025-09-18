using System.Drawing;

// Get first argument
var fileName = args.First();
Console.WriteLine($"Opening image {fileName}");

// Load image
var image = new Bitmap(fileName);
int totalPixels = image.Width * image.Height;

// Array to hold distances
int[,] distances = new int[image.Width, image.Height];

// Iterate over every pixel in the image
for (int y = 0; y < image.Height; y++)
{
    for (int x = 0; x < image.Width; x++)
    {
        // Set initial distance to 0 for black pixels, max for others
        var pixel = image.GetPixel(x, y);
        distances[x, y] = pixel.R == 0 && pixel.G == 0 && pixel.B == 0 ? 0 : int.MaxValue;
    }
}

// Forward pass
for (int y = 1; y < image.Height; y++)
{
    for (int x = 1; x < image.Width; x++)
    {
        // Skip black pixels
        if (distances[x, y] == 0) continue;

        // Check pixel to left
        distances[x, y] = Math.Min(distances[x, y], distances[x - 1, y] + 1);
        // Check pixel above
        distances[x, y] = Math.Min(distances[x, y], distances[x, y - 1] + 1);
    }
}

// Backward pass
for (int y = image.Height - 2; y >= 0; y--)
{
    for (int x = image.Width - 2; x >= 0; x--)
    {
        // Skip black pixels
        if (distances[x, y] == 0) continue;

        // Check pixel to right
        distances[x, y] = Math.Min(distances[x, y], distances[x + 1, y] + 1);
        // Check pixel below
        distances[x, y] = Math.Min(distances[x, y], distances[x, y + 1] + 1);
    }
}

// Iterate over every pixel to set color based on distance
for (int y = 0; y < image.Height; y++)
{
    for (int x = 0; x < image.Width; x++)
    {
        int distance = distances[x, y];
        if (distance > 0 && distance < int.MaxValue)
        {
            int value = Math.Min(distance / 2, 255);
            image.SetPixel(x, y, Color.FromArgb(255, value, value));
        }
    }
}

// Save image
var outputFileName = Path.GetFileNameWithoutExtension(fileName) + "_output.png";
image.Save(outputFileName);
Console.WriteLine($"Saved output image to {outputFileName}");