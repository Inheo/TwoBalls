void RotateAboutAxisRadians_float(float3 In, float3 Axis, float Rotation, out float3 Out)
{
    float s = sin(Rotation);
    float c = cos(Rotation);
    float one_minus_c = 1.0 - c;

    Axis = normalize(Axis);

    float3x3 rot_mat = {
        one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s,
        one_minus_c * Axis.z * Axis.x + Axis.y * s,
        one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c,
        one_minus_c * Axis.y * Axis.z - Axis.x * s,
        one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s,
        one_minus_c * Axis.z * Axis.z + c
    };

    Out = mul(rot_mat, In);
}

void Morph_float(in float3 position, in float3 center, in float3 deformation,
                 in float rotationMultiplier, in float offsetMultiplier, out float3 result)
{
    if (dot(deformation, deformation) < 0.0000001)
    {
        result = position;
        return;
    }

    float3 radiusVector = position - center;
    float radius = length(radiusVector);

    float3 rotationAxis = normalize(cross(deformation, radiusVector));
    float3 rotationDir = normalize(cross(rotationAxis, radiusVector));

    float3 deformationDir = normalize(deformation);
    float rotationRate = abs(dot(rotationDir, deformationDir));
    float offsetRate = 1.0 - rotationRate;

    float rotationAngle = abs(dot(deformation, rotationDir)) * rotationMultiplier * radius * rotationRate;
    RotateAboutAxisRadians_float(radiusVector, rotationAxis, rotationAngle, radiusVector);

    radiusVector += deformation * (offsetMultiplier * offsetRate * radius);

    result = center + radiusVector;
}
