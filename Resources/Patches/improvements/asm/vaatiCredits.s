.thumb
push	{r4,r5,lr}
mov	r4,r0
ldrb	r0,[r4,#0x0E]
sub	r0,#1
strb	r0,[r4,#0x0E]
push	{r0-r2}
ldr	r0,=#0x3001002
mov	r1,#4
strb	r1,[r0]
mov	r1,#0
strb	r1,[r0,#1]
strb	r1,[r0,#2]
pop	{r0-r2}
ldr	r3,=#0x8041D0B
bx	r3
