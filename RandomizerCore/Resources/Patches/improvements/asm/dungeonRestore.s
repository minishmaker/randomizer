.thumb
ldr	r0,=#0x2033A90
ldr	r2,=#0x3000BF0
ldrb	r1,[r2,#4]
cmp	r1,#0x2C
bne	notarmos
ldrb	r1,[r2,#5]
cmp	r1,#9
blo	notarmos
mov	r1,#3
b	store
notarmos:
ldrb	r1,[r2,#4]
lsl	r1,#2
ldr	r2,=#0x80528E8
ldr	r2,[r2]
add	r1,r2
ldrb	r1,[r1,#1]
sub	r1,#0x17
store:
strb	r1,[r0,#3]
ldr	r3,=#0x806FFEC
mov	lr,r3
.short	0xF800
cmp	r0,#0
ldr	r3,=#0x8051805
bx	r3
