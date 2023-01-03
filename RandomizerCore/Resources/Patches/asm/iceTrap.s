.thumb
ldr	r1,=#0x3001160
ldr	r2,=#0x3003F80
add	r1,#0x45
mov	r0,#0x9C
strb	r0,[r1]
ldr	r0,=#0x800
str	r0,[r2,#0x30]
mov	r0,#0xD
strb	r0,[r2,#0xC]

ldr	r1,=#0x3001160
add	r1,#0x42
mov	r0,#0xC
strb	r0,[r1]
sub	r1,#5
mov	r0,#0x10
strb	r0,[r1]
add	r1,#9
ldr	r0,=#0x280
strh	r0,[r1]	

end:
bx	lr
