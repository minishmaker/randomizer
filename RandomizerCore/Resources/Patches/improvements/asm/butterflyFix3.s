.thumb
push	{r4,lr}
mov	r4,r0
add	r0,#0x41
ldrb	r1,[r0]
mov	r0,#1
strb	r0,[r4,#0x18]
ldr	r3,=#0x809F245
bx	r3
