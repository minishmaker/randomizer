.thumb
cmp	r7,#0xFF
bhi	vanilla
cmp	r7,#0
beq	vanilla
mov	r1,r7
sub	r1,#0x17
b	new
vanilla:
ldr	r0,=#0x2033A90
ldrb	r1,[r0,#3]
new:
ldr	r0,=#0x2002E9C
add	r1,r0
ldrb	r0,[r1]
add	r0,r4
ldr	r3,=#0x805232D
bx	r3
