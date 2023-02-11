.thumb
push	{r4,lr}
mov	r4,r0

@check if custom rupee boy
ldrb	r0,[r0,#0xB]
cmp	r0,#0
beq	norupee

@follow link
ldr	r0,=#0x3001160
add	r0,#0x2C
ldr	r1,[r0]
ldr	r2,[r0,#4]
mov	r0,r4
add	r0,#0x80
str	r1,[r0]
str	r2,[r0,#4]

@check if we are on a fourth frame
ldr	r0,=#0x300100C
ldrb	r0,[r0]
mov	r1,#7
and	r0,r1
cmp	r0,#0
bne	norupee

@remove a rupee
ldr	r0,=#0x2002B00
ldr	r1,[r0]
cmp	r1,#0
beq	norupee
sub	r1,#1
str	r1,[r0]

norupee:
ldr	r1,=#0x8085C90
ldr	r1,[r1]
ldrb	r0,[r4,#0x0C]
ldr	r3,=#0x8085C69
bx	r3
