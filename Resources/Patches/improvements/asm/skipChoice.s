.thumb
ldsb	r7,[r0,r7]
ldr	r0,=#0x3000FF0
ldrh	r1,[r0,#2]
ldrh	r0,[r0]
ldr	r2,=#0xFF0F
and	r0,r2
orr	r0,r1
cmp	r0,#8
ldr	r3,=#0x8056565
bx	r3
