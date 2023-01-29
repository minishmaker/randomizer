.thumb
@check if hurdy
ldr	r3,=#0x2002C2D
cmp	r3,r0
bne	noHurdy
@check if the fusion has been done before
ldr	r3,=#0x2002C87
ldrb	r3,[r3]
lsr	r3,#2
lsl	r3,#0x1F
cmp	r3,#0
bne	noHurdy
mov	r5,#0x32
noHurdy:
strb	r5,[r0]
sub	r2,#0x80
add	r0,r1,r2
add	r0,r8
ldr	r3,=#0x801EA45
bx	r3
