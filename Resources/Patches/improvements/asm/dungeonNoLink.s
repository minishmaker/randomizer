.thumb
ldr	r3,=#0x2033A90
ldrb	r3,[r3,#3]
ldr	r1,=#0x2032EE0
ldrb	r1,[r1]
lsl	r1,#2
ldr	r2,=#0x80528E8
ldr	r2,[r2]
add	r1,r2
ldrb	r1,[r1,#1]
sub	r1,#0x17
cmp	r3,r1
beq	match
ldr	r0,=#0x6014000
ldr	r1,=#0x6014060
mov	r2,#0
loop:
str	r2,[r0]
add	r0,#4
cmp	r0,r1
bne	loop
mov	r0,#0x12

match:
mov	r8,r0
ldr	r5,=#0x3001010
mov	r0,#0x80
lsl	r0,#3
ldr	r3,=#0x80A5535
bx	r3
