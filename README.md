# Image Edge Distance
This is a simple program to draw gradients on images based on the manhattan distance from any "fixed colour" pixel. This is better explained through the example images below.

Example images are copyrighted and may not be used or redistributed for any purpose other than demonstrating the functionality of this program.

## Example
This is used as the input image. It has a black background with white shapes.

![Example input, an image with a black background and white shapes](examples/example.png)

The "fixed color" is the black pixels. The non-fixed pixels (i.e. the white pixels) will have a gradient drawn over them, based on the distance from any fixed (black) pixel. The gradient is green to white, so pixels closest to a fixed (black) pixel become green, and as the distance from any fixed (black) pixel increases, the pixels become whiter.

![Example output, where the previously white areas now have a green to white gradient](examples/example_output.png)

## Example 2
The resulting image from the previous example is now used as the input image. The fixed pixels are now set to any pixel containing green, which means that any black pixel is not fixed. The gradient is blue to black.

![Example output 2, where the previously black areas now have a blue to black gradient](examples/example_output_output.png)

## Usage
The code is intended to be modified so that you can configure the fixed colours and the gradient.

### Configuration
#### Fixed colours
The fixed colour will not be modified by the program. Any pixels satisfying this condition will be used in distance calculations.

Here are example conditions corresponding to the above examples.
```cs
static bool IsFixedColor(Color color)
{
    // Black pixels
    return color.R == 0 && color.G == 0 && color.B == 0;
}
```
```cs
static bool IsFixedColor(Color color)
{
    // Any green pixels
    return color.G > 0;
}
```

#### Result colours
The result colour is used to draw the gradient. The distance value is passed in, and will always be a number 1-255 inclusive.

Here are example gradients corresponding to the above examples.
```cs
static Color GetResultColor(int value)
{
    // Green to white
    return Color.FromArgb(value, 255, value);
}
```
```cs
static Color GetResultColor(int value)
{
    // Blue to black
    return Color.FromArgb(0, 0, 255 - value);
}
```

#### Divisor
As the RGB range is limited to 0-255 inclusive, this means that a gradient normally cannot extend beyond 256 pixels.

To get around this limitation, you can set a divisor, which stretches the gradient.

For example...
1 = gradient drawn over 256 pixels
2 = gradient drawn over 512 pixels
4 = gradient drawn over 1024 pixels

### Running
Use .NET 9 on Windows to run this program. The easiest way is to have the .NET SDK installed, and use `dotnet run <file name>` while in the folder containing the program code. The resulting image will have `_output` appended to its filename.

No other steps are needed to build the program - just clone this repo, edit the code as explained above, and run this command.

For example, `dotnet run image.png`. The output image will be called `image_output.png`.

It may take more than a minute to process very large images.
