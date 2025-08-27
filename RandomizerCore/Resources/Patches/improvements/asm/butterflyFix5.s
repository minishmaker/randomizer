.thumb
ldrb	r0,[r2,#0x0F]
cmp	r0,#16
blo	continueFlyingUp

ldr	r3,=#0x809F3AF
bx	r3

continueFlyingUp:
add	r0,#1
strb	r0,[r2,#0x0F]
ldr	r3,=#0x809F3D5
bx	r3
