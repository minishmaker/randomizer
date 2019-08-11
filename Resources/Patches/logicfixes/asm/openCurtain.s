.thumb
push	{r4-r7,lr}
mov	r6,r0
add	r0,#0x86
ldrh	r0,[r0]
push	{r0}
ldr	r0,=#0x2002C9C
ldrb	r0,[r0]
mov	r1,#4
and	r0,r1
cmp	r0,#0
beq	noChu
ldr	r0,=#0x2002CEC
ldrb	r1,[r0]
mov	r2,#8
orr	r1,r2
strb	r1,[r0]
noChu:
pop	{r0}
ldr	r3,=#0x807C608
mov	lr,r3
.short	0xF800
ldr	r3,=#0x8091931
bx	r3
