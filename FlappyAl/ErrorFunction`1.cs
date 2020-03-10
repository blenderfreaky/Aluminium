using System;

namespace FlappyAl
{
    public delegate double ErrorFunction<T>(T[] actual, T[] expected);
}
