.thumb

ldrh	r3, [r5, #0x14]
lsr	r3, #1
lsl	r3, #1
strh	r3, [r5, #0x14]

ldr	r3, =#0x80700C0
mov	lr, r3
.short	0xF800
mov	r0, #0x01
pop	{pc}

