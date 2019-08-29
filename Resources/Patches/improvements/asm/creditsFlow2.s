.thumb
ldrh	r0,[r4,#8]
ldr	r3,=#0x200008C
ldr	r2,[r3]
ldr	r3,=#0x812743C
cmp	r2,r3
bhs	vanilla
ldr	r3,=#0x8127370
cmp	r2,r3
bhi	check2
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
mov	r2,#8
and	r2,r3
cmp	r2,#0
bne	skip
ldr	r3,=#0x3000FF0
ldrh	r2,[r3,#2]
ldrh	r3,[r3,#4]
orr	r3,r2
mov	r2,#1
and	r2,r3
cmp	r2,#0
bne	special
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
ldr	r2,=#0x100
and	r2,r3
cmp	r2,#0
beq	vanilla
cmp	r0,#16
bls	special
sub	r0,#16
b	store
vanilla:
sub	r0,#1
b	store
special:
mov	r0,#0
b	store
skip:
ldr	r2,=#0x8127370
ldr	r3,=#0x200008C
str	r2,[r3]
mov	r0,#0
store:
strh	r0,[r4,#8]
lsl	r0,#0x10
ldr	r3,=#0x80A3025
bx	r3

check2:
ldr	r3,=#0x3000FF0
ldrh	r3,[r3]
mov	r2,#8
and	r2,r3
cmp	r2,#0
beq	vanilla
ldr	r2,=#0x812743C
ldr	r3,=#0x200008C
str	r2,[r3]
mov	r0,#0
b	store
