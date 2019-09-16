.thumb
ldrb	r4,[r5,#10]
ldr	r0,=#0x800DA5A
ldrb	r0,[r0]
strb	r0,[r5,#10]
mov	r0,r5
ldr	r3,=#0x8095159
bx	r3
