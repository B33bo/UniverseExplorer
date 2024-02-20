#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void Modulus_float(float2 A, float B, out float2 Out) {
    float x = A.x - floor(A.x / B) * B;
    float y = A.y - floor(A.y / B) * B;
    Out = float2(x, y);
}

void ModulusFlip_float(float2 A, float B, out float2 Out) {
    float2 sizePastPoint = floor(A / B);

    Out = A - sizePastPoint * B;

    // flips every 2nd item
    Out.x = abs(sizePastPoint.x % 2 - Out.x);
    Out.y = abs(sizePastPoint.y % 2 - Out.y);
}
#endif