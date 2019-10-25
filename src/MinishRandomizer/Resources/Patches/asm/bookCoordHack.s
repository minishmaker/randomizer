.thumb
mov	r0,r12
ldr	r3,[r0,#0x48]
ldr	r4,[r5,#0x48]
ldr	r1,=#0x81146D0
cmp	r1,r4
bne	np
ldr	r3,=#0x80FC694
np:
ldrb	r1,[r3,#6]
ldrb	r0,[r4,#6]
ldr	r2,=#0x80177F7
bx	r2
