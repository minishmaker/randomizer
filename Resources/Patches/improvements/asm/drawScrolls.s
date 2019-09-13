.equ scrollTable, graphics+4
.thumb
push	{r4-r7,lr}
mov	r7,r8
push	{r7}
ldr	r6,=#0x3001010
push	{r0-r7}

@load the graphics
ldr	r0,graphics
ldr	r1,=#0x600C400
ldr	r2,=#0x600E000
loop:
ldr	r3,[r0]
str	r3,[r1]
add	r0,#4
add	r1,#4
cmp	r1,r2
blo	loop

@check if we have fast spin
mov	r0,#0x73
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nospin
ldr	r0,=#0x1020
ldr	r1,=#0x600F8C2
bl	drawIcon

@check if we have fast split
nospin:
mov	r0,#0x74
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nosplit
ldr	r0,=#0x102F
ldr	r1,=#0x600F982
bl	drawIcon

@check if we have long great spin
nosplit:
mov	r0,#0x75
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nolong
ldr	r0,=#0x103E
ldr	r1,=#0x600F930
bl	drawIcon

nolong:
@for every scroll, check if we have it, if we have it, check if it is on or not
ldr	r4,scrollTable
ldr	r5,=#0x600F8C4
mov	r6,#0	@current loop, for the horizontal offset
scrollLoop:
ldrb	r0,[r4,r6]
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#1
and	r1,r0
cmp	r1,#0
beq	invisible
mov	r1,#2
and	r1,r0
cmp	r1,#0
beq	on
b	off

on:
ldr	r0,=#0x204D
mov	r1,#5
mul	r1,r6
add	r0,r1
b	drawScroll

off:
ldr	r0,=#0x0075
mov	r1,#5
mul	r1,r6
add	r0,r1
b	drawScroll

invisible:
ldr	r0,=#0x0020
b	drawScroll

drawScroll:
mov	r1,r5
bl	drawStatus
add	r5,#6
add	r6,#1
cmp	r6,#2
beq	moveRight
cmp	r6,#6
beq	moveRight
cmp	r6,#8
bne	scrollLoop
b	switch
moveRight:
add	r5,#2
b	scrollLoop

switch:
@check if we own this scroll
ldr	r0,=#0x20000AE
ldrb	r6,[r0]
ldrb	r0,[r4,r6]
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	end
@check if select is pressed
ldr	r0,=#0x3000FF0
ldrh	r0,[r0,#2]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	end
@check if off
ldrb	r0,[r4,r6]
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
mov	r1,#2
and	r0,r1
cmp	r0,#0
beq	turnoff
@turn on
ldrb	r0,[r4,r6]
ldr	r1,=#0x2002B44
sub	r0,#0x48
lsl	r2,r0,#1
lsr	r2,#3
add	r1,r2
push	{r0,r1}
lsl	r0,#1
mov	r1,#8
swi	#6
mov	r2,r1
add	r2,#1
pop	{r0,r1}
mov	r3,#1
lsl	r3,r2
mvn	r3,r3
ldrb	r0,[r1]
and	r0,r3
strb	r0,[r1]
b	update
@turn off
turnoff:
ldrb	r0,[r4,r6]
ldr	r1,=#0x2002B44
sub	r0,#0x48
lsl	r2,r0,#1
lsr	r2,#3
add	r1,r2
push	{r0,r1}
lsl	r0,#1
mov	r1,#8
swi	#6
mov	r2,r1
add	r2,#1
pop	{r0,r1}
mov	r3,#1
lsl	r3,r2
ldrb	r0,[r1]
orr	r0,r3
strb	r0,[r1]
b	update

update:
ldr	r3,=#0x807A910
mov	lr,r3
.short	0xF800

end:
pop	{r0-r7}
ldr	r3,=#0x80A5979
bx	r3

drawIcon:
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
add	r0,#1
add	r1,#0x40
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
add	r0,#1
strh	r0,[r1,#6]
add	r0,#1
strh	r0,[r1,#8]
bx	lr

drawStatus:
strh	r0,[r1]
add	r0,#1
strh	r0,[r1,#2]
add	r0,#1
strh	r0,[r1,#4]
bx	lr

.align
.ltorg
graphics:
