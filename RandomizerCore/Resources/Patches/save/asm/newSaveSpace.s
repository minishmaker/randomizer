.thumb
push	{r0-r7}
mov	r7,r0

@check if writing to backup
ldr	r0,=#0x1000
cmp	r0,r7
bhs	vanilla

@check if titlescreen
ldr	r0,=#0x3001002
ldrb	r0,[r0]
cmp	r0,#1
bne	saveSave

@check if making new save
ldr	r0,=#0x2032EC2
ldrb	r1,[r0]
cmp	r1,#1
beq	eraseSave

@check if making a copy
cmp	r1,#5
beq	copyGame

@check if erasing save
cmp	r1,#6
beq	eraseSave
b	end

copyGame:
ldr	r0,=#0x2019EE6
ldrb	r0,[r0]	@save we are copying
ldr	r1,=#0x2019EE7
ldrb	r1,[r1]	@save we are copying to
ldr	r2,=#0x500
mul	r0,r2
mul	r1,r2
ldr	r3,=#0xE001100
add	r0,r3
add	r1,r3
mov	r3,#0
copyLoop:
ldrb	r4,[r0,r3]
strb	r4,[r1,r3]
add	r3,#1
cmp	r3,r2
bne	copyLoop
b	end

saveSave:
@check so we save only once
ldr	r0,=#0x1080
cmp	r0,r7
beq	match
ldr	r0,=#0x1580
cmp	r0,r7
beq	match
ldr	r0,=#0x1A80
cmp	r0,r7
bne	end
match:
@increase times saved
ldr	r0,=#0x203FE00
ldrh	r1,[r0,#8]
ldr	r2,=#0xFFFF
cmp	r1,r2
beq	noincrease
add	r1,#1
noincrease:
strh	r1,[r0,#8]
@and save
ldr	r0,=#0x203FB00
ldr	r1,=#0x2000004
ldrb	r1,[r1]
ldr	r2,=#0x500
mul	r1,r2
ldr	r3,=#0xE001100
add	r1,r3
mov	r3,#0
b	copyLoop

eraseSave:
ldr	r0,=#0x2019EE6
ldrb	r0,[r0]
ldr	r2,=#0xE001100
ldr	r1,=#0x500
mul	r0,r1
add	r0,r2	@save offset
mov	r2,#0
mov	r3,#0
eraseLoop:
strb	r2,[r0,r3]
add	r3,#1
cmp	r3,r1
bne	eraseLoop
b	end

end:
pop	{r0-r7}
bx	lr

vanilla:
pop	{r0-r7}
push	{r4-r7,lr}
mov	r6,r0
mov	r7,r1
lsr	r5,r2,#3
ldr	r0,=#0x807CC39
bx	r0
