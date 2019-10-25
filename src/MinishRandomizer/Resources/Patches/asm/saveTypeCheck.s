.equ badSave2, badSave1+4
.equ badSave3, badSave2+4
.equ badEmu1, badSave3+4
.equ badEmu2, badEmu1+4
.equ badEmu3, badEmu2+4
.equ badEmu4, badEmu3+4
.thumb
push	{r4,r5}
@emulator detection
@first we do a dma transfer from valid memory to valid memory
ldr	r4,=#0x40000C8
ldr	r0,=#0x8000000
ldr	r1,=#0x2000000
mov	r2,#8
str	r0,[r4]
str	r1,[r4,#4]
strh	r2,[r4,#8]
ldrh	r5,=#0x8000
strh	r5,[r4,#10]
@then we do a dma transfer from invalid memory to valid memory
ldr	r0,=#0x0000000
str	r0,[r4]
strh	r5,[r4,#10]
@and now we check if the result was 0, if so we are dealing with a bad emulator
ldr	r0,=#0x2000000
ldr	r1,[r0]
cmp	r1,#0
bne	checksave

bl	loadFont

@write the message
ldr	r4,=#0x6000182
ldr	r0,badEmu1
mov	r1,r4
bl	draw
add	r4,#0xC2
ldr	r0,badEmu2
mov	r1,r4
bl	draw
add	r4,#0x44
ldr	r0,badEmu3
mov	r1,r4
bl	draw
add	r4,#0xBE
ldr	r0,badEmu4
mov	r1,r4
bl	draw

mov	r4,#0
sleeps:
swi	#5
add	r4,#1
cmp	r4,#180
bne	sleeps

ldr	r4,=#0x4000130
ldr	r5,=#0x3FF
button:
swi	#5
ldrh	r0,[r4]
cmp	r0,r5
beq	button

@save type detection
checksave:
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

bl	loadFont

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

sleep:
swi	#5
b	sleep

end:
pop	{r4,r5}
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

loadFont:
swi	#5
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
bx	lr

.align
.ltorg
badSave1:
@POIN badSave1
@POIN badSave2
@POIN badSave3
@POIN badEmu1
@POIN badEmu2
@POIN badEmu3
@POIN badEmu4
