.thumb
ldrb	r0,[r1,#2]
ldrb	r1,[r1,#3]
mov	r2,#0
cmp	r0,#0x1B
beq	isTrap
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
b	end

isTrap:
cmp	r1,#0xFF
bne	notRandom
ldr	r0,table
mov	r1,#0
randomLoop:
ldr	r2,[r0]
add	r1,#1
add	r0,#8
cmp	r2,#0
bne	randomLoop
ldr	r0,=#0x300100C
ldrb	r0,[r0]
swi	#6
notRandom:
ldr	r0,table
lsl	r1,#3
push	{r0,r1}
ldr	r0,[r0,r1]
cmp	r0,#0
beq	end
mov	lr,r0
.short	0xF800
pop	{r0,r1}
add	r1,r0
ldr	r1,[r0,#4]
mov	r0,#0
sub	r0,r1
ldr	r3,=#0x80522BC
mov	lr,r3
.short	0xF800

end:
ldr	r3,=#0x8083A81
bx	r3
.align
.ltorg
table:
