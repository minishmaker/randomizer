.thumb
@check if this is a figurine
cmp	r6,#0x67
beq	figurine

@check if this is a kinstone
cmp	r6,#0x5C
beq	kinstone

@check if this is a dungeon item
checkKey:
cmp	r6,#0x50
blo	end
cmp	r6,#0x53
bhi	end

@check if sub id is 0
cmp	r7,#0
beq	end

@otherwise, get text id based on sub id
cmp	r7,#0x18
beq	dws
cmp	r7,#0x19
beq	cof
cmp	r7,#0x1A
beq	fow
cmp	r7,#0x1B
beq	tod
cmp	r7,#0x1C
beq	pow
cmp	r7,#0x1D
beq	dhc
cmp	r7,#0x1E
beq	rc
b	end

dws:
ldr	r0,=#0x720
b	return

cof:
ldr	r0,=#0x721
b	return

fow:
ldr	r0,=#0x722
b	return

tod:
ldr	r0,=#0x723
b	return

pow:
ldr	r0,=#0x725
b	return

dhc:
ldr	r0,=#0x727
b	return

rc:
ldr	r0,=#0x724
b	return

kinstone:
cmp	r7,#0x65
beq	tornado
cmp	r7,#0x66
beq	tornado
cmp	r7,#0x67
beq	tornado
cmp	r7,#0x68
beq	tornado
cmp	r7,#0x69
beq	tornado
cmp	r7,#0x6A
beq	totem
cmp	r7,#0x6B
beq	totem
cmp	r7,#0x6C
beq	totem
cmp	r7,#0x6D
beq	crown
b	end

tornado:
ldr	r0,=#0x71A
b	return

totem:
ldr	r0,=#0x70D
b	return

crown:
ldr	r0,=#0x717
b	return

end:
mov	r0,r8
return:
pop	{r3}
mov	r8,r3
pop	{r4-r7,pc}

figurine:
push	{r4-r7}
@check if the we already had this figurine
ldr	r0,=#0x2002B0E
mov	r1,r7
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	notnewfigurine

@increase figurine total
ldr	r0,=#0x2002AF0
ldrb	r1,[r0]
cmp	r1,#0xFF
beq	maxfigurines
add	r1,#1
strb	r1,[r0]
maxfigurines:

@set the figurine as collected
ldr	r0,=#0x2002B0E
mov	r1,r7
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

@do the text
notnewfigurine:
ldr	r4,=#0x0800
orr	r4,r7
ldr	r5,=#0x0900
orr	r5,r7
ldr	r7,=#0x203F200	@offset
@write the figurine name in red
mov	r0,#2
strb	r0,[r7]
mov	r0,#1
strb	r0,[r7,#1]
add	r7,#2
mov	r0,r4
bl	getTextWrap
mov	r1,r7
bl	writeText
mov	r7,r1
mov	r0,#2
strb	r0,[r7]
mov	r0,#0
strb	r0,[r7,#1]
add	r7,#2

@add : and two newlines
mov	r0,#0x3A
strb	r0,[r7]
mov	r0,#0x0A
strb	r0,[r7,#1]
strb	r0,[r7,#2]
add	r7,#3

@and write the figurine description
mov	r0,r5
bl	getTextWrap
mov	r1,r7
bl	writeText

@ram text
ldr	r0,=#0x2C05
pop	{r4-r7}
b	return

writeText:
ldrb	r2,[r0]
strb	r2,[r1]
cmp	r2,#0
beq	endwrite
cmp	r2,#0xF
bhi	notcode
cmp	r2,#3
blo	shortcode
cmp	r2,#5
bhi	shortcode
add	r0,#1
add	r1,#1
ldrb	r2,[r0]
strb	r2,[r1]
shortcode:
add	r0,#1
add	r1,#1
ldrb	r2,[r0]
strb	r2,[r1]
notcode:
add	r0,#1
add	r1,#1
b	writeText
endwrite:
bx	lr

getTextWrap:
push	{lr}
ldr	r3,=#0xFFFF
cmp	r0,r3
bhi	isoffset
ldr	r3,getTextOffset
mov	lr,r3
.short	0xF800
pop	{pc}
isoffset:
ldr	r3,=#0x2000007
ldrb	r3,[r3]
lsl	r3,#2
ldr	r0,[r0,r3]
pop	{pc}

.ltorg
.align
getTextOffset:
