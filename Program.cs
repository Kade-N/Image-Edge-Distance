using System.Drawing;

// Returns true if this color should remain as-is.
// Modify to change which pixels are considered "fixed".
// The distance will be calculated to the nearest fixed pixel.
static bool IsFixedColor(Color color)
{
    // Black pixels
    return color.R == 0 && color.G == 0 && color.B == 0;
}

// Returns color based on distance value.
// Modify to change the color gradient.
static Color GetResultColor(int value)
{
    // Green to white
    return Color.FromArgb(value, 255, value);
}

// Divides distance value over a larger area, making the effect more gradual.
// Increase to make the effect more gradual, decrease to make it sharper.
const int DIVISOR = 2;



// Get first argument
var fileName = args.First();
Console.WriteLine($"Processing image {fileName}");

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
        // Set initial distance to 0 for fixed pixels, max for others
        var pixel = image.GetPixel(x, y);
        distances[x, y] = IsFixedColor(pixel) ? 0 : int.MaxValue;
    }
}

// Forward pass
for (int y = 1; y < image.Height; y++)
{
    for (int x = 1; x < image.Width; x++)
    {
        // Skip fixed pixels
        if (distances[x, y] == 0) continue;

        // Check pixel to left
        if (distances[x - 1, y] < int.MaxValue)
        {
            distances[x, y] = Math.Min(distances[x, y], distances[x - 1, y] + 1);
        }

        // Check pixel above
        if (distances[x, y - 1] < int.MaxValue)
        {
            distances[x, y] = Math.Min(distances[x, y], distances[x, y - 1] + 1);
        }
    }
}

// Backward pass
for (int y = image.Height - 2; y >= 0; y--)
{
    for (int x = image.Width - 2; x >= 0; x--)
    {
        // Skip fixed pixels
        if (distances[x, y] == 0) continue;

        // Check pixel to right
        if (distances[x + 1, y] < int.MaxValue)
        {
            distances[x, y] = Math.Min(distances[x, y], distances[x + 1, y] + 1);
        }

        // Check pixel below
        if (distances[x, y + 1] < int.MaxValue)
        {
            distances[x, y] = Math.Min(distances[x, y], distances[x, y + 1] + 1);
        }
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
            int value = Math.Min(distance / DIVISOR, 255);
            image.SetPixel(x, y, GetResultColor(value));
        }
    }
}



// Save image
var outputFileName = Path.GetFileNameWithoutExtension(fileName) + "_output.png";
image.Save(outputFileName);
Console.WriteLine($"Saved output image to {outputFileName}");