.thumb
cmp	r0,#7
beq	ghost
lsl	r0,#2
add	r0,r1
ldr	r1,[r0]
sub	r1,#1
mov	r0,#1
lsl	r0,r1
end:
ldrh	r1,[r2]
orr	r0,r1
strh	r0,[r2]
bx	lr

ghost:
mov	r0,#0x21
b	end
