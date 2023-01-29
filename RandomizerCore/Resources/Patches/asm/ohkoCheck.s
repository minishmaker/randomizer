.thumb
push	{lr}
push	{r0-r7}
ldr	r2,=#0x80000000
and	r2,r0
cmp	r2,#0
beq	nodeath
ldr	r3,custom
cmp	r3,#0
beq	death
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	nodeath

death:
pop	{r0-r7}
ldr	r1,=#0x2002AE8
mov	r0,#0
strb	r0,[r1,#2]
ldr	r1,=#0x3001160
add	r1,#0x45
strb	r0,[r1]
pop	{pc}

nodeath:
pop	{r0-r7}
ldr	r2,=#0x2002AE8
ldrb	r1,[r2,#2]
add	r1,r0
ldr	r3,=#0x80522C5
bx	r3
.align
.ltorg
custom:
