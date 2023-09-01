# ShaderToy入门

[shadertoy](https://www.shadertoy.com/)
[IQ大神的入门视频](https://www.youtube.com/watch?v=0ifChJ0nJfM)
[IQ大神的博客](https://iquilezles.org/)
[Shadertoy Tutorial](https://inspirnathan.com/posts/47-shadertoy-tutorial-part-1/)

[图形计算机](https://www.desmos.com/calculator?lang=zh-CN)

## 数学工具

[desmos](https://www.desmos.com/calculator?lang=zh-CN)
[DesmosMatrix](https://www.desmos.com/matrix?lang=zh-CN)
[wolframalpha](https://www.wolframalpha.com/)

## Inigo Quilez入门

使用 alt + enter来编译

smoothstep(a, b, x)函数：将x小于a的部分变为0，大于b的部分变为1
mix(a, b, x)：输出 (1.0 - x) * a + x * b

```c
void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;
    vec2 dis = uv - vec2(0.35,0.7);
    // Time varying pixel color
    //vec3 col = 0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));
    vec3 col = mix(vec3(1, 0.4, 0.1), vec3(1.0, 0.8, 0.3), sqrt(uv.y));

    float r = 0.2 + 0.1 * cos(atan(dis.y, dis.x)*10.0 + dis.x * 20.0 + 1.5);
    col *= smoothstep(r, r + 0.01, length(dis));
    
    r = 0.015;
    r += 0.002*sin(120.0*dis.y);
    r += exp(-35.0*uv.y);
    col *= 1.0 - (1.0 - smoothstep(r, r+0.002, abs(dis.x - 0.25*sin(2.0*dis.y)))) * (1.0 - smoothstep(0.0, 0.1, dis.y));

    // Output to screen
    fragColor = vec4(col,1.0);
}
```

## APIs

### Step

step(a, b):
b > a ? 1 : 0

