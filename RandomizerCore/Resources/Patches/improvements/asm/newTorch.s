.thumb
push	{r4,lr}
mov	r4,r0

ldrb	r0,[r4,#0x1F]
cmp	r0,#0
beq	start

ldrh	r0,[r4,#0x3E]
cmp	r0,#0x16
bne	notright
ldr	r0,=#0x20333D0
ldr	r0,[r0]
cmp	r0,#0
bne	notright
mov	r0,#0x16
ldr	r3,=#0x807C6EC
mov	lr,r3
.short	0xF800
notright:

ldrh	r0,[r4,#0x3E]
cmp	r0,#0x15
bne	notleft
ldr	r0,=#0x2033390
ldr	r0,[r0]
cmp	r0,#0
bne	notleft
mov	r0,#0x15
ldr	r3,=#0x807C6EC
mov	lr,r3
.short	0xF800
notleft:

start:
ldrb	r0,[r4,#0x1F]
cmp	r0,#0
bne	alreadystarted
mov	r0,#0x1E
strb	r0,[r4,#0x0F]
strb	r0,[r4,#0x1F]
alreadystarted:

mov	r0,r4
ldr	r3,=#0x805B7F8
mov	lr,r3
.short	0xF800

mov	r0,r4
ldr	r3,=#0x805B728
mov	lr,r3
.short	0xF800
pop	{r4,pc}
