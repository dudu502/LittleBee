# unity-simple-URP-pixelation

Custom renderer feature to pixelate the screen
Code Walkthrough: [youtube video](https://youtu.be/nvIyQHbhTJE)

### Disclaimer
I'm experimenting with this effect to not have to use two separate cameras as in my previous [approach](https://github.com/itsPeetah/PixelatedCamera).
The code might not be optimized since I don't really know how the RendererFeatures operate in depth.

## Usage
To use this, go to your renderer settings and add the PixelizeFeature. Set your target height and you're on :)

Commented code in the shader and in the PixelizePass class is for alternate versions of this effect that either:
- Don't use the shader
- Don't down scale the render texture in the first blit pass

Un-comment at your own risk lmao

## Resources
- Custom render passes (by @alexanderameye): https://alexanderameye.github.io/notes/scriptable-render-passes/
- Custom render passes: https://learn.unity.com/tutorial/custom-render-passes-with-urp
- Create a custom renderer feature: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.1/manual/containers/create-custom-renderer-feature-1.html
- SamplerStates in shader lab: https://docs.unity3d.com/Manual/SL-SamplerStates.html

