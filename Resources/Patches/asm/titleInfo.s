.equ	string2, string1+4
.equ	string3, string2+4
.equ	string4, string3+4
.equ	sword, string4+4
.thumb
lsl	r0,#4
strh	r0,[r5,#8]
ldrb	r0,[r5,#5]
add	r0,#1
strb	r0,[r5,#5]
push	{r0-r4}

@wait a frame to avoid junk
swi	#5

@turn on bg3
ldr	r0,=#0x3000F50
ldr	r3,=#0x4000000
ldr	r1,=#0x1F40
strh	r1,[r0,#0x00]
strh	r1,[r3,#0x00]
ldr	r1,=#0x1F0C
strh	r1,[r0,#0x2C]
strh	r1,[r3,#0x0E]
mov	r1,#4
strh	r1,[r0,#0x30]
strh	r1,[r3,#0x1E]

@fix bg2
ldr	r1,=#0x3C89
strh	r1,[r0,#0x20]
strh	r1,[r3,#0x0C]
ldr	r0,sword
ldr	r1,=#0x600E000
ldr	r2,=#0x600E800
swordloop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	swordloop

@set registers up
ldr	r4,=#0x600FB00

@and draw each string
ldr	r0,string1
mov	r1,r4
bl	draw
add	r4,#0x40
ldr	r0,string2
mov	r1,r4
bl	draw
add	r4,#0x40
ldr	r0,string3
mov	r1,r4
bl	draw
add	r4,#0x40
ldr	r0,string4
mov	r1,r4
bl	draw

end:
pop	{r0-r4}
ldr	r3,=#0x80ACDDF
bx	r3

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

.align
.ltorg
string1:
