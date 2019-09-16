.thumb
ldrb	r4,[r5,#10]

@check room
ldr	r3,=#0x3000BF0
ldrb	r0,[r3,#4]
cmp	r0,#0x68
bne	vanilla
ldrb	r0,[r3,#5]
cmp	r0,#0
beq	ghost

vanilla:
mov	r0,#0x5C
strb	r0,[r5,#10]
mov	r0,r5
ldr	r3,=#0x8095159
bx	r3

ghost:
ldr	r0,=#0x800DA5A
ldrb	r0,[r0]
strb	r0,[r5,#10]
mov	r0,r5
ldr	r3,=#0x8095159
bx	r3
