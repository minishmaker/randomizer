.equ badSave2, badSave1+4
.equ badSave3, badSave2+4
.thumb
ldr	r0,=#0xE000000
ldrb	r1,[r0]
ldrb	r2,[r0,#1]
lsl	r2,#8
orr	r1,r2
ldrb	r2,[r0,#2]
lsl	r2,#16
orr	r1,r2
ldrb	r2,[r0,#3]
lsl	r2,#24
orr	r1,r2
ldr	r0,=#0x454C4441
cmp	r1,r0
beq	end

@load the debug font
ldr	r0,=#0x85C2F70
ldr	r1,=#0x6000800
ldr	r2,=#0x6001000
debug:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
bne	debug

@make a palette for the debug font
ldr	r0,=#0x5000000
ldr	r1,=#0x7FFF
strh	r1,[r0,#0x1C]
ldr	r1,=#0x7A00
strh	r1,[r0]
strh	r1,[r0,#0x1E]
ldr	r0,=#0x6000C8C
ldr	r1,=#0xFEF0
strh	r1,[r0]

@write the message
ldr	r4,=#0x60001C2
ldr	r0,badSave1
mov	r1,r4
bl	draw
add	r4,#0x46
ldr	r0,badSave2
mov	r1,r4
bl	draw
add	r4,#0xBE
ldr	r0,badSave3
mov	r1,r4
bl	draw

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

sleep:
swi	#5
b	sleep

end:
ldr	r3,=#0x8055C34
mov	lr,r3
.short	0xF800
ldr	r3,=#0x8055D88
mov	lr,r3
.short	0xF800
ldr	r1,=#0x2000010
ldr	r3,=#0x8055A05
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
add	r2,#0x20
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
badSave1:
@POIN badSave1
@POIN badSave2
@POIN badSave3
