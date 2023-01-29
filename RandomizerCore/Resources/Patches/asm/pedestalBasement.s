.thumb
push	{lr}
ldr	r0,=#0x2002D0B
ldrb	r0,[r0]
mov	r1,#1
and	r0,r1
ldr	r3,=#0x804BA25
bx	r3
