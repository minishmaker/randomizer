.thumb
cmp	r7,#0
beq	vanilla
mov	r0,r7
sub	r0,#0x17
b	new
vanilla:
ldrb	r0,[r0,#3]
new:
add	r1,r0
ldrb	r0,[r1]
ldrb	r2,[r4,#2]
orr	r0,r2
ldr	r3,=#0x8053E13
bx	r3
