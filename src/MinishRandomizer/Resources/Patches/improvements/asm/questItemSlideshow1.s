.thumb
add	r0,r1,r7
cmp	r5,#0x34
blo	vanilla
cmp	r5,#0x3C
bhi	vanilla

ldrb	r3,[r0]
cmp	r3,#0
beq	vanilla
cmp	r3,r5
beq	vanilla

ldr	r2,=#0x2000096
mov	r1,#6
ldrb	r3,[r2]
cmp	r3,r5
beq	match
add	r1,#1
add	r2,#1
ldrb	r3,[r2]
cmp	r3,r5
beq	match
add	r1,#1
add	r2,#1
ldrb	r3,[r2]
cmp	r3,r5
beq	match
ldr	r3,=#0x80A4DFF
bx	r3

match:
mov	r0,r2

vanilla:
strb	r5,[r0]
lsl	r1,#3
mov	r0,#0xE0
ldr	r3,=#0x80A4DF5
bx	r3

