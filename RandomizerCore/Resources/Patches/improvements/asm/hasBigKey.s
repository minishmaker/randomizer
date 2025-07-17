
.thumb
@check if this is about the DHC big key
cmp	r1,#0x06
bne	universal

add	r0,r1

universal:
ldrb	r1,[r0]
mov	r0,#0x4
and	r0,r1
ldr	r3,=#0x805239D
bx	r3
