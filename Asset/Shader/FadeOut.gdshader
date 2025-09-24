shader_type canvas_item;
uniform float radius : hint_range(0.0, 1.0) = 1.0; // 원 크기
uniform vec2 center = vec2(0.5, 0.5); // 화면 중앙 (UV 좌표)

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	 vec2 norm = UV - center;
    float dist = length(norm);

    if (dist < radius) {
        COLOR  = vec4(0.0, 0.0, 0.0, 0.0); // 투명 → 화면 보임
    } else {
        COLOR  = vec4(0.0, 0.0, 0.0, 1.0); // 검은색 → 덮기
    }
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
