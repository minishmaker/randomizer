.thumb
push	{r4,r5,lr}
mov	r4,r0
ldrb	r0,[r4,#0x0A]
mov	r3,#1
strb	r3,[r4,#0x18]
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x809AC61
bx	r3
