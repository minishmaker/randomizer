.thumb
@r0 is string
@r1 is x
@r2 is y
@r3 is offset or 0

cmp	r3,#0
bne	custom
@get offset
lsl	r1,#1
mov	r3,#0x40
mul	r2,r3
add	r1,r2
ldr	r2,=#0x600F800
add	r1,r2
b	draw

custom:
mov	r1,r3

draw:
ldr	r3,=#0x8EF3340
cmp	r0,r3
blo	vanilla
ldr	r3,=#0x2000007
ldrb	r3,[r3]
lsl	r3,#2
ldr	r0,[r0,r3]
vanilla:
mov	r3,#0
drawloop:
ldrb	r2,[r0,r3]
cmp	r2,#0
beq	endDraw
cmp	r2,#0x20
bls	space
sub	r2,#0x2F
b	np
space:
mov	r2,#0
np:
strh	r2,[r1]
add	r1,#2
nextDraw:
add	r3,#1
cmp	r3,#0x1E
bne	drawloop
endDraw:
bx	lr
