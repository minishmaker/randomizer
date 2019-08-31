.thumb
@r0 is string
@r1 is x
@r2 is y

@get offset
lsl	r1,#1
mov	r3,#0x40
mul	r2,r3
add	r1,r2
ldr	r2,=#0x600F800
add	r1,r2

draw:
mov	r3,#0
drawloop:
ldrb	r2,[r0,r3]
cmp	r2,#0
beq	endDraw
cmp	r2,#0x20
bls	space
cmp	r2,#0x60
blo	np
cmp	r2,#0x60
beq	space
cmp	r2,#0x7A
bhi	space
sub	r2,#0x20
b	np
space:
mov	r2,#0x20
np:
strh	r2,[r1]
add	r1,#2
nextDraw:
add	r3,#1
cmp	r3,#0x1E
bne	drawloop
endDraw:
bx	lr
