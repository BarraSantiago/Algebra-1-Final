#include <iostream>
#include <cmath>
#include <raylib.h>
#include <raymath.h>


using namespace std;

struct Vector
{
    Vector2 from;
    Vector2 to;
};

constexpr int maxVectors = 4;

// Calcula el punto de interseccion entre 2 vectores
Vector2 vectorIntersection(Vector vector1, Vector vector2);

// dibujar los vectores
void DrawVector(Vector v1, Color color);

int main()
{
    InitWindow(720 * 1.5f, 480 * 1.5f, "algebra");
    
    // Almacenamos los puntos de interseccion encontrados
    Vector2 intersectionPoints[maxVectors] = {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}};
    Vector vectors[maxVectors];
    Color colors[maxVectors] = {WHITE, RED, GREEN, BLUE};


    vectors[0] = {{316, 173}, {120, 387}};
    vectors[1] = {{82, 400.0f}, {350.0f, 270.0f}};
    vectors[2] = {{550.0f, 400.0f}, {300.0f, 260.0f}};
    vectors[3] = {{270.0f, 130.0f}, {550.0f, 450.0f}};


    int intersectionCount = 0;

    // Buscamos los puntos de interseccion
    for (int i = 0; i < maxVectors; ++i)
    {
        for (int j = i + 1; j < maxVectors; ++j)
        {
            Vector2 intersection = vectorIntersection(vectors[i], vectors[j]);
            if (intersection.x >= 0)
            {
                // Verificar que el punto de interseccion no este duplicado
                bool duplicate = false;
                for (int k = 0; k < intersectionCount; k++)
                {
                    if (intersectionPoints[k].x == intersection.x && intersectionPoints[k].y == intersection.y)
                    {
                        duplicate = true;
                        break;
                    }
                }
                if (!duplicate)
                {
                    intersectionPoints[intersectionCount++] = intersection;
                }
            }
        }
    }

    if (intersectionCount == 4)
    {
        cout << "Los vectores forman un cuadrilatero." << endl;

        // Calculamos el area del cuadrilatero utilizando la formula de Brahmagupta
        float a = Vector2Distance(intersectionPoints[0], intersectionPoints[1]);
        float b = Vector2Distance(intersectionPoints[1], intersectionPoints[2]);
        float c = Vector2Distance(intersectionPoints[2], intersectionPoints[3]);
        float d = Vector2Distance(intersectionPoints[3], intersectionPoints[0]);
        float s = (a + b + c + d) / 2;
        float area = sqrt((s - a) * (s - b) * (s - c) * (s - d));
        cout << "El area del cuadrilatero es: " << area << endl;
    }
    else
    {
        cout << "Los vectores no forman un cuadrilatero." << endl;
    }


    while (!WindowShouldClose())
    {
        BeginDrawing();
        ClearBackground(BLACK);

        for (int i = 0; i < maxVectors; ++i)
        {
            DrawVector(vectors[i], colors[i]);
        }
        for (int i = 0; i < maxVectors; ++i)
        {
            DrawCircle(static_cast<int>(intersectionPoints[i].x),
                       static_cast<int>(intersectionPoints[i].y), 5.0f, YELLOW);
        }
        EndDrawing();
    }

    CloseWindow();
    return 0;
}

void DrawVector(Vector v1, Color color)
{
    DrawLine(static_cast<int>(v1.from.x), static_cast<int>(v1.from.y), static_cast<int>(v1.to.x),
             static_cast<int>(v1.to.y), color);
    DrawLineEx(v1.from, v1.to, 0.5, color);
    DrawLine(static_cast<int>(v1.from.x), static_cast<int>(v1.from.y), static_cast<int>(v1.to.x),
             static_cast<int>(v1.to.y), color);
}

// Funcion para calcular el punto de interseccion entre dos vectores
Vector2 vectorIntersection(Vector vector1, Vector vector2)
{
    // Calcula los coeficientes
    float a1 = vector1.to.y - vector1.from.y;
    float b1 = vector1.from.x - vector1.to.x;
    float c1 = a1 * (vector1.from.x) + b1 * (vector1.from.y);

    float a2 = vector2.to.y - vector2.from.y;
    float b2 = vector2.from.x - vector2.to.x;
    float c2 = a2 * (vector2.from.x) + b2 * (vector2.from.y);

    // Calcula el determinante para verificar si hay una solucion unica
    float determinant = a1 * b2 - a2 * b1;

    if (determinant == 0)
    {
        return {-2, -2};
    }

    float x = (b2 * c1 - b1 * c2) / determinant;
    float y = (a1 * c2 - a2 * c1) / determinant;

    // Verifica si el punto de interseccion está dentro de los segmentos de linea
    if (min(vector1.from.x, vector1.to.x) <= x && x <= max(vector1.from.x, vector1.to.x)
        && min(vector1.from.y, vector1.to.y) <= y && y <= max(vector1.from.y, vector1.to.y)
        && min(vector2.from.x, vector2.to.x) <= x && x <= max(vector2.from.x, vector2.to.x)
        && min(vector2.from.y, vector2.to.y) <= y && y <= max(vector2.from.y, vector2.to.y))
    {
        return {x, y};
    }

    return {-3, -3};
}
